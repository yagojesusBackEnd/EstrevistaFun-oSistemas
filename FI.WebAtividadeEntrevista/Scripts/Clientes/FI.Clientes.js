$(document).ready(function () {
    var clienteId = $('#clienteId').val();

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        var clienteData = {
            Id: clienteId,
            Nome: $('#Nome').val(),
            Sobrenome: $('#Sobrenome').val(),
            CEP: $('#CEP').val(),
            Email: $('#Email').val(),
            Nacionalidade: $('#Nacionalidade').val(),
            CPF: $('#CPF').val(),
            Estado: $('#Estado').val(),
            Cidade: $('#Cidade').val(),
            Logradouro: $('#Logradouro').val(),
            Telefone: $('#Telefone').val()
        };

        var beneficiarios = [];
        $('#beneficiariosList tr').each(function () {
            var cpf = $(this).find('.cpf').text();
            var nome = $(this).find('.nome').text();
            beneficiarios.push({ CPF: cpf, Nome: nome });
        });

        $.ajax({
            url: urlPost,
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                Cliente: clienteData, 
                Beneficiarios: beneficiarios
            }),
            success: function (response) {
                alert("Cliente e beneficiários salvos com sucesso!");
                window.location.href = urlRetorno;
            },
            error: function () {
                alert("Erro ao salvar o cliente e beneficiários.");
            }
        });
    });
});
