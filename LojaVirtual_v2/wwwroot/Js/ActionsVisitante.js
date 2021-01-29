$(document).ready(function () {
    MoverScroll();
    MudarOrdenacao();
    MudarImagemPrincipalProduto();
    MudarQuantidadeProdutoCarrinho();
    AJAXBuscarCEP();
    AcaoCalcularFrete();
    MascaraCEP();
    AJAXCalcularFrete(false);

    AJAXEnderecoEntregaCalcularFrete();

});

function AJAXEnderecoEntregaCalcularFrete() {
    $("input[name=endereco]").change(function () {

        $.cookie("Carrinho.Endereco", $(this).val(), { path: "/" });

        var cep = RemoverMascara($(this).parent().find("input[name=cep]").val());

        EnderecoEntregaCardsLimpar();
        LimparValores();
        EnderecoEntregaCardsLoading();
        $(".btn-continuar").addClass("disabled");


        $.ajax({
            type: "GET",
            url: "/CarrinhoCompra/CalcularFrete?cepDestino=" + cep,
            error: function (data) {
                MostrarMensagemDeErro("Opps! Tivemos um erro ao obter o Frete..." + data.Message);

                EnderecoEntregaCardsLimpar();
            },
            success: function (data) {
                EnderecoEntregaCardsLimpar();

                for (var i = 0; i < data.listaValores.length; i++) {
                    var tipoFrete = data.listaValores[i].tipoFrete;
                    var valor = data.listaValores[i].valor;
                    var prazo = data.listaValores[i].prazo;

                    $(".card-title")[i].innerHTML = "<label for='" + tipoFrete + "'>" + tipoFrete + "</label>";
                    $(".card-text")[i].innerHTML = "<label for='" + tipoFrete + "'>Prazo de " + prazo + " dias.</label>";
                    $(".card-footer .text-muted")[i].innerHTML = "<input type=\"radio\" name=\"frete\" value=\"" + tipoFrete + "\" id='" + tipoFrete + "' /> <strong><label for='" + tipoFrete + "'>" + numberToReal(valor) + "</label></strong>";

                    console.info($.cookie("Carrinho.TipoFrete") + " - " + tipoFrete)
                    console.info($.cookie("Carrinho.TipoFrete") == tipoFrete);

                    if ($.cookie("Carrinho.TipoFrete") != undefined && $.cookie("Carrinho.TipoFrete") == tipoFrete) {
                        $(".card-footer .text-muted input[name=frete]").eq(i).attr("checked", "checked");
                        SelecionarTipoFreteStyle($(".card-footer .text-muted input[name=frete]").eq(i));

                        $(".btn-continuar").removeClass("disabled");
                    }
                }

                $(".card-footer .text-muted").find("input[name=frete]").change(function () {
                    $.cookie("Carrinho.TipoFrete", $(this).val(), { path: '/' });
                    $(".btn-continuar").removeClass("disabled");

                    SelecionarTipoFreteStyle($(this));

                });
            }
        });
    });
}



function SelecionarTipoFreteStyle(obj) {

    
    $(".card-body").css("background-color", "white");
    $(".card-footer").css("background-color", "rgba(0,0,0,0.03)");

    obj.parent().parent().parent().find(".card-body").css("background-color", "#D7EAFF");
    obj.parent().parent().parent().find(".card-footer").css("background-color", "#D7EAFF");
    AtualizarValores();
}

function AtualizarValores() {

    var produto = parseFloat($(".texto-produto").text().replace("R$", "").replace(".", "").replace(",", "."));

    var frete = parseFloat($(".card-footer input[name=frete]:checked").parent().find("label").text().replace("R$", "").replace(".", "").replace(",", "."));

    var total = produto + frete;
    
    $(".texto-frete").text(numberToReal(frete));
    $(".texto-total").text(numberToReal(total));
}

function LimparValores() {
    $(".texto-frete").text("-");
    $(".texto-total").text("-");
}

function EnderecoEntregaCardsLoading() {
    for (var i = 0; i < 3; i++) {
        $(".card-text")[i].innerHTML = "<br /><br /><img style:'width: 60px; height: 60px' src = '\\img\\preloader.gif' />";
    }
}

function EnderecoEntregaCardsLimpar() {
    for (var i = 0; i < 3; i++) {

        $(".card-title")[i].innerHTML = "-";
        $(".card-text")[i].innerHTML = "-";
        $(".card-footer .text-muted")[i].innerHTML = "-";
    }
}

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

function AJAXBuscarCEP() {
    $("#CEP").keyup(function () {
        OcultarMensagemErro();
        if ($(this).val().length == 10) {

            var cep = RemoverMascara($(this).val());
            $.ajax({
                type: "GET",
                url: "https://viacep.com.br/ws/" + cep + "/json/?callback=callback_name",
                dataType: "jsonp",
                error: function (data) {
                    MostrarMensagemErro("Opps! Tivemos um erro na busca pelo CEP! Tente mais tarde!");
                },
                success: function (data) {
                    if (data.erro == undefined) {
                        $("#Estado").val(data.uf);
                        $("#Cidade").val(data.localidade);
                        $("#Complemento").val(data.complemento);
                        $("#Endereco").val(data.logradouro);
                        $("#Bairro").val(data.bairro);
                    } else {
                        MostrarMensagemErro("O CEP informado não existe!");
                    }

                }
            });
        }
    });
}

function MascaraCEP() {
    $(".cep").mask("00.000-000");
}

function RemoverMascara(valor) {
    return valor.replace(".", "").replace("-", "");
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

    if ($(".cep").length > 0) {



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
                    console.info(data);
                    html = "";
                    for (var i = 0; i < data.listaValores.length; i++) {
                        var tipoFrete = data.listaValores[i].tipoFrete;
                        var valor = data.listaValores[i].valor;
                        var prazo = data.listaValores[i].prazo;

                        html += "<dl class=\"dlist-align\"><dt><input type=\"radio\" name=\"frete\" value=\"" + tipoFrete + "\" /> <input type=\"hidden\" name=\"valor\" value=\"" + valor + "\" /> </dt><dd>" + tipoFrete + " - " + numberToReal(valor) + " (" + prazo + " dias úteis)</dd></dl>";
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
