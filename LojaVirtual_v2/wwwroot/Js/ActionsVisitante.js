$(document).ready(function () {
    MoverScroll();
    MudarOrdenacao();
    MudarImagemPrincipalProduto();
    MudarQuantidadeProdutoCarrinho();

    AcaoCalcularFrete();
    MascaraCEP();
    AJAXCalcularFrete(false);
});

function MudarQuantidadeProdutoCarrinho() {
    $("#order .btn-primary").click(function () {

        if ($(this).hasClass("aumentar")) {
            OrquestradorDeAcoesProduto("aumentar", $(this));
        }
        if ($(this).hasClass("diminuir")) {
            OrquestradorDeAcoesProduto("diminuir", $(this));
        }
    });
}


function MascaraCEP() {
    $(".cep").mask("00.000-000");
}

function AcaoCalcularFrete() {
    $(".btn-calcular-frete").click(function (e) {
        //Requisição ajax de frete
        AJAXCalcularFrete(true);
        e.preventDefault();
    });
}

function AJAXCalcularFrete(callByButton) {
    $(".btn-continuar").addClass("disabled");
    if (callByButton == false) {
        if ($.cookie('Carrinho.Cep') != undefined) {
            $(".cep").val($.cookie('Carrinho.Cep'));
        }
    }
  
    var cep = $(".cep").val().replace(".", "").replace("-", "");
    $.removeCookie("Carrinho.TipoFrete");
    if (cep.length == 8) {

        $.cookie('Carrinho.Cep', $(".cep").val());
        $(".container-frete").html("<br /><br /><img style:'width: 60px; height: 60px' src='\\img\\preloader.gif'/>");
        $(".frete").text("R$ 00,00");
        $(".total").text("R$ 00,00");

        $.ajax({
            type: "GET",
            url: "/CarrinhoCompra/CalcularFrete?cepDestino=" + cep,
            error: function (data) {
                MostrarMensagemErro("Opps! Tivemos um erro ao obter o Frete ..." + data.Message)
                console.info(data);
            },
            success: function (data) {

                html = "";
                for (var i = 0; i < data.length; i++) {
                    var tipoFrete = data[i].tipoFrete;
                    var valor = data[i].valor;
                    var prazo = data[i].prazo;

                    html += "<dl class=\"dlist-align\"><dt><input type=\"radio\" name=\"frete\" value=\"" + tipoFrete + "\" /> <input type=\"hidden\" name=\"valor\" value=\""+ valor+"\" /> </dt><dd>" + tipoFrete + " - " + numberToReal(valor) + " (" + prazo + " dias úteis)</dd></dl>";
                }

                $(".container-frete").html(html);
                $(".container-frete").find("input[type=radio]").change(function () {

                    $.cookie("Carrinho.TipoFrete", $(this).val());
                    $(".btn-continuar").removeClass("disabled");

                    var valorFrete = parseFloat($(this).parent().find("input[type=hidden]").val());

                    $(".frete").text(numberToReal(valorFrete));
                   
                    var subtotal = parseFloat($(".subtotal").text().replace("R$", "").replace(".", "").replace(",", "."));
                    var total = valorFrete + subtotal;
                    $(".total").text(numberToReal(total));
                });
                console.info(data);
            }
        });
    } else {
        if (callByButton) {
            $(".container-frete").html("");
            MostrarMensagemErro("Digite o CEP para calcular o frete!");
        } 
    }
}

function OrquestradorDeAcoesProduto(operacao, botao) {
    OcultarMensagemErro();
    /*
     * Carregamento dos valores
     * */

    var pai = botao.parent().parent();

    var produtoId = pai.find(".inputProdutoId").val();
    var quantidadeEstoque = parseInt(pai.find(".inputQuantidadeEstoque").val());
    var valorUnitario = parseFloat(pai.find(".inputValorUnitario").val().replace(",", "."));

    var campoQuantidadeProdutoCarrinho = pai.find(".inputQuantidadeProdutoCarrinho");
    var quantidadeProdutoCarrinhoAntiga = parseInt(campoQuantidadeProdutoCarrinho.val());

    var campoValor = botao.parent().parent().parent().parent().parent().find(".price");

    var produto = new ProdutoQuantidadeEValor(produtoId, quantidadeEstoque, valorUnitario, quantidadeProdutoCarrinhoAntiga, 0, campoQuantidadeProdutoCarrinho, campoValor);
    /**
     * Chamada de metodos 
     **/
    AlteracoesVisuaisProdutoCarrinho(produto, operacao);

    //Validacao


    //TODO - atualizar o sub valor do produt
}

function AlteracoesVisuaisProdutoCarrinho(produto, operacao) {
    if (operacao == "aumentar") {

        //if (produto.quantidadeProdutoCarrinhoAntiga != produto.quantidadeEstoque) {

        produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga + 1;
        AtualizarQuantidadeEValor(produto);
        AJAXComunicarAlteracaoQuantidadeProduto(produto);
    } else if (operacao == "diminuir") {

        //if (produto.quantidadeProdutoCarrinhoAntiga != 1) {

        produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga - 1;
        AtualizarQuantidadeEValor(produto);
        AJAXComunicarAlteracaoQuantidadeProduto(produto);
    }
}

function AJAXComunicarAlteracaoQuantidadeProduto(produto) {
    $.ajax({
        type: "GET",
        url: "/CarrinhoCompra/AlterarItem?id=" + produto.produtoId + "&quantidade=" + produto.quantidadeProdutoCarrinhoNova,
        error: function (data) {
            MostrarMensagemErro(data.responseJSON.mensagem);
            //RollBack
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga;
            AtualizarQuantidadeEValor(produto);
        },
        success: function () {
            AJAXCalcularFrete();
        }
    });
}

function MostrarMensagemErro(mensagem) {
    $(".alert-danger").css("display", "block");
    $(".alert-danger").text(mensagem);
}

function OcultarMensagemErro() {
    $(".alert-danger").css("display", "none");
}

function AtualizarQuantidadeEValor(produto) {

    produto.campoQuantidadeProdutoCarrinho.val(produto.quantidadeProdutoCarrinhoNova);
    var resultado = numberToReal(produto.valorUnitario * produto.quantidadeProdutoCarrinhoNova);
    produto.campoValor.text(resultado);

    AtualizarSubTotal();
}

function AtualizarSubTotal() {
    var Subtotal = 0;
    var TagsPrice = $(".price");

    TagsPrice.each(function () {
        var ValorReais = parseFloat($(this).text().replace("R$", "").replace(".", "").replace(" ", "").replace(",", "."));
        Subtotal += ValorReais;
    })

    $(".subtotal").text(numberToReal(Subtotal));

}

function numberToReal(numero) {
    var numero = numero.toFixed(2).split('.');
    numero[0] = "R$ " + numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}


function MudarImagemPrincipalProduto() {
    $(".img-small-wrap img").click(function () {
        var caminho = $(this).attr("src");
        $(".img-big-wrap img").attr("src", caminho);
        $(".img-big-wrap a").attr("href", caminho);
    });
}

function MudarOrdenacao() {
    $("#ordenacao").change(function () {

        var Pagina = 1;
        var Pesquisa = "";
        var Ordenacao = $(this).val();
        var Fragmento = "#posicao-produto";

        var QueryString = new URLSearchParams(window.location.search);
        if (QueryString.has("pagina")) {
            Pagina = QueryString.get("pagina");
        }
        if (QueryString.has("pesquisa")) {
            Pesquisa = QueryString.get("pesquisa");
        }

        if ($("#breadcrumb").length > 0) {
            Fragmento = "";
        }

        var URL = window.location.protocol + "//" + window.location.host + window.location.pathname;
        //Home/index?pagina=&pesquisa=&ordenacao=& = 
        var URLComParametros = URL + "?pagina=" + Pagina + "&pesquisa=" + Pesquisa + "&ordenacao=" + Ordenacao + Fragmento;
        window.location.href = URLComParametros;

    });
}

function MoverScroll() {

    if (window.location.hash.length > 0) {
        var hash = window.location.hash;
        if (hash == "#posicao-produto") {
            window.scrollBy(0, 420);
        }
    }
}

/**
 * Classes
 * */


class ProdutoQuantidadeEValor {
    constructor(produtoId, quantidadeEstoque, valorUnitario, quantidadeProdutoCarrinhoAntiga, quantidadeProdutoCarrinhoNova, campoQuantidadeProdutoCarrinho, campoValor) {
        this.produtoId = produtoId;
        this.quantidadeEstoque = quantidadeEstoque;
        this.valorUnitario = valorUnitario;

        this.quantidadeProdutoCarrinhoAntiga = quantidadeProdutoCarrinhoAntiga;
        this.quantidadeProdutoCarrinhoNova = quantidadeProdutoCarrinhoNova;

        this.campoQuantidadeProdutoCarrinho = campoQuantidadeProdutoCarrinho;
        this.campoValor = campoValor;
    }
}
