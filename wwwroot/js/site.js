/*Toast Javascript*/
$(document).ready(function () {

    var message = $('#AlertMessage').text();
    if (message !== '') {
        $('.toast').toast('show')
    }
});

/*Mascara input*/
$(document).ready(function () {

    $('#Telefone').mask('(00) 00000-0000', { placeholder: "(xx) xxxxx-xxxx" });
    $('#Cpf').mask('000.000.000-00', { reverse: true, placeholder: "xxx.xxx.xxx-xx" });
    $('#CPF').mask('000.000.000-00', { reverse: true, placeholder: "xxx.xxx.xxx-xx" });
});

/*JS IMAGEM*/
function exibirImagem() {
    var oFReader = new FileReader();
    oFReader.readAsDataURL(document.getElementById("Imagem").files[0]);
    oFReader.onload = function (oFREvent) {
        document.getElementById("imgPreview").src = oFREvent.target.result;
    };
}

/*AJAX Aula*/
function aplicaFiltroConsultaAvancadaAula() {
    var vIdMateria = document.getElementById('materia-dropdown').value;
    var vDataInicial = document.getElementById('data-inicial').value;
    var vDataFinal = document.getElementById('data-final').value;
    var vSituacao = document.getElementById('situacao-dropdown').value;

    $.ajax({
        url: "/ConsultaAulas/ConsultaAulaAvancada",
        data: { dataInicial: vDataInicial, dataFinal: vDataFinal, idMateria: vIdMateria, situacao: vSituacao },
        success: function (dados) {
            if (dados.erro != undefined) {
                alert(dados.msg);
            }
            else {
                document.getElementById('resultadoConsulta').innerHTML = dados;
            }
        },
    });
}

/*AJAX Listagem*/
function aplicaFiltroConsultaAvancadaListagem() {
    var vCodigo = document.getElementById('codigo').value;
    var vtipoTabela = document.getElementById('tipoTabela').value;
    $.ajax({
        url: "/ConsultaListagens/ObtemDadosConsultaAvancada",
        data: { codigo: vCodigo, tipoTabela: vtipoTabela },
        success: function (dados) {
            if (dados.erro != undefined) {
                alert(dados.msg);
            }
            else {
                document.getElementById('resultadoConsulta').innerHTML = dados;
            }
        },
    });
}

/*Abrir Modal Imagem*/
$(document).on("click", ".openImageDialog", function () {
    console.log('Ok');
    var myImageId = $(this).data('id');
    $(".modal-body #myImage").attr("src", myImageId);
});

/*Deletar Dados*/
function apagarAluno(id) {
    if (confirm('Confirma a exclusão do registro?'))
        location.href = '/Aluno/Delete?id=' + id;
}

function apagarProfessor(id) {
    if (confirm('Confirma a exclusão do registro?'))
        location.href = '/Professor/Delete?id=' + id;
}

function apagarMateria(id) {
    if (confirm('Confirma a exclusão do registro?'))
        location.href = '/Materia/Delete?id=' + id;
}

function apagarAula(id) {
    if (confirm('Confirma a exclusão do registro?'))
        location.href = '/Aula/Delete?id=' + id;
}
