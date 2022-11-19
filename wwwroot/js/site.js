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

/*Pagination table*/
$(document).ready(function () {
    $('#table-presenca').DataTable();
});

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


/*AJAX Presencas*/
function aplicaFiltroConsultaAvancadaPresencas() {
    var vIdAluno = document.getElementById('aluno-dropdown').value;
    var vIdAula = document.getElementById('aula-dropdown').value;
    $.ajax({
        url: "/ConsultaPresencas/ConsultaPresencaAvancada",
        data: { idAluno: vIdAluno, idAula: vIdAula },
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
    Swal.fire({
        title: 'Confirma a exclusão do registro?',
        text: "Você não poderá reverter essa ação!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#007bff',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim'
    }).then((result) => {
        if (result.isConfirmed) {
            location.href = '/Aluno/Delete?id=' + id;
        }
    })
}

function apagarProfessor(id) {
    Swal.fire({
        title: 'Confirma a exclusão do registro?',
        text: "Você não poderá reverter essa ação!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#007bff',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim'
    }).then((result) => {
        if (result.isConfirmed) {
            location.href = '/Professor/Delete?id=' + id;
        }
    })
}

function apagarMateria(id) {
    Swal.fire({
        title: 'Confirma a exclusão do registro?',
        text: "Você não poderá reverter essa ação!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#007bff',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim'
    }).then((result) => {
        if (result.isConfirmed) {
            location.href = '/Materia/Delete?id=' + id;
        }
    })
}

function apagarAula(id) {
    Swal.fire({
        title: 'Confirma a exclusão do registro?',
        text: "Você não poderá reverter essa ação!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#007bff',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim'
    }).then((result) => {
        if (result.isConfirmed) {
            location.href = '/Aula/Delete?id=' + id;
        }
    })
}
