
$(document).ready(function () {

    $(".btn-danger").click(function (e) {

        var resultado = confirm("Tem certeza que deseja realizar esta operação?");
        if (resultado == false) {
            e.preventDefault();
        }
    });
    $('.dinheiro').mask('000.000.000.000.000,00', { reverse: true });

    AjaxUploadImagemProduto();
});

function AjaxUploadImagemProduto() {
    $(".img-upload").click(function () {
        $(this).parent().find(".input-file").click();
    });

    
    $(".btn-imagem-excluir").click(function () {
       
        var Imagem = $(this).parent().find(".img-upload");
        var CampoHidder = $(this).parent().find("input[name=imagem]");
        var BtnExcluir = $(this).parent().find(".btn-imagem-excluir");
        var InputFile = $(this).parent().find(".input-file");
        $.ajax({
            type: "GET",
            url: "/Colaborador/Imagem/Deletar?caminho=" + CampoHidder.val(),
            error: function () {
                alert("Erro no envio do arquivo");
            },
            success: function () {
                Imagem.attr("src", "/img/logoOnePiece.png");
                BtnExcluir.addClass("btn-ocultar");
                CampoHidder.val("");
                InputFile.val("");
            }
        });
    });


    $(".input-file").change(function () {

        //Formulario de dados via js

        var Binario = $(this)[0].files[0];
        var formulario = new FormData();
        formulario.append("file", Binario);

        var CampoHidden = $(this).parent().find("input[name=imagem]");
        var Imagem = $(this).parent().find(".img-upload");
        var BtnExcluir = $(this).parent().find(".btn-imagem-excluir");
        //Requisicao ajax enviando o formulario
        $.ajax({
            type: "POST",
            url: "/Colaborador/Imagem/Armazenar",
            data: formulario,
            contentType: false,
            processData: false,
            error: function () {
                alert("Erro no envio do arquivo");
            },
            success: function(data){
                var caminho = data.caminho;
                Imagem.attr("src", caminho);
                CampoHidden.val(caminho);
                BtnExcluir.removeClass("btn-ocultar");
            }

        });
    });
}