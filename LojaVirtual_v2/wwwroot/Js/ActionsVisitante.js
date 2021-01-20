$(document).ready(function () {
    MoverScroll();
    MudarOrdenacao();
    MudarImagemPrincipalProduto();
});

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
            //window.scrollBy(0, 420);
        }
    }
}