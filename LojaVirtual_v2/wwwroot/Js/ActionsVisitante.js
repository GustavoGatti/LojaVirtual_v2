$(document).ready(function () {
    MoverScroll();
    MudarOrdenacao();
    MudarImagemPrincipalProduto();
    MudarQuantidadeProdutoCarrinho();
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
        
            produto.quantidadeProdutoCarrinhoNova =  produto.quantidadeProdutoCarrinhoAntiga + 1;
            AtualizarQuantidadeEValor(produto);
            AJAXComunicarAlteracaoQuantidadeProduto(produto);
    } else if (operacao == "diminuir") {

        //if (produto.quantidadeProdutoCarrinhoAntiga != 1) {
          
            produto.quantidadeProdutoCarrinhoNova =  produto.quantidadeProdutoCarrinhoAntiga - 1;
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
