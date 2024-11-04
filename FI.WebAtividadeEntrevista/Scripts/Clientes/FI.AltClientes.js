$(document).ready(function () {
    // Preenche o formulário caso haja dados do cliente
    if (obj) {
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(obj.CEP);
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(obj.Telefone);
        $('#formCadastro #CPF').val(obj.CPF);
    }

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        // Coleta os dados do formulário
        const clienteData = {
            Id: obj.Id,  // Inclui o ID do cliente
            Nome: $('#Nome').val(),
            CEP: $('#CEP').val(),
            Email: $('#Email').val(),
            Sobrenome: $('#Sobrenome').val(),
            Nacionalidade: $('#Nacionalidade').val(),
            Estado: $('#Estado').val(),
            Cidade: $('#Cidade').val(),
            Logradouro: $('#Logradouro').val(),
            Telefone: $('#Telefone').val(),
            CPF: $('#CPF').val()
        };

        // Coleta os dados dos beneficiários da tabela
        const beneficiarios = [];
        $('#beneficiariosList tr').each(function () {
            const cpf = $(this).find('.cpf').text();
            const nome = $(this).find('.nome').text();
            if (cpf && nome) {
                beneficiarios.push({ CPF: cpf, Nome: nome });
            }
        });

        // Exibe no console para verificar o conteúdo dos beneficiários
        console.log("Dados do cliente:", clienteData);
        console.log("Beneficiários:", beneficiarios);

        // Envia os dados do cliente via AJAX
        $.ajax({
            url: urlPost, // URL de salvar
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({ cliente: clienteData, beneficiarios: beneficiarios }), // Inclui beneficiários
            success: function (r) {
                ModalDialog("Sucesso!", "Cliente e beneficiários salvos com sucesso!", function () {
                    window.location.href = urlRetorno; // Redireciona após salvar
                });
            },
            error: function (r) {
                const errorMessage = r.status === 400 ? r.responseJSON : "Ocorreu um erro interno no servidor.";
                ModalDialog("Ocorreu um erro", errorMessage);
            }
        });
    });
});

// Função para exibir o modal de diálogo
function ModalDialog(titulo, texto, onClose) {
    const randomId = `modal_${Math.random().toString().replace('.', '')}`;
    const modalHtml = `
        <div id="${randomId}" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">${titulo}</h4>
                    </div>
                    <div class="modal-body">
                        <p>${texto}</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>
                    </div>
                </div>
            </div>
        </div>`;

    $('body').append(modalHtml);
    const modal = $(`#${randomId}`);
    modal.modal('show');

    // Remove o modal do DOM após ser fechado e chama a função onClose, se existir
    modal.on('hidden.bs.modal', function () {
        modal.remove();
        if (onClose) onClose();
    });
}
