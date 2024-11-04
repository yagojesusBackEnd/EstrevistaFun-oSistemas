$(document).ready(function () {
    var editRow = null;
    var clienteId = $('#clienteId').val();
    var beneficiariosTemporarios = [];

    $('#beneficiarioCPF').mask('000.000.000-00');

    $('#btnBeneficiarios').on('click', function () {
        $('#beneficiariosModal').modal('show');
        limparCamposModal();

        // Carrega apenas beneficiários persistidos
        if (clienteId) {
            $.ajax({
                url: `/Beneficiario/BeneficiarioListById`,
                method: 'GET',
                data: { idCliente: clienteId },
                success: function (data) {
                    $('#beneficiariosList').empty();

                    // Itera sobre os beneficiários persistidos e adiciona à lista temporária se ainda não estiverem lá
                    data.Records.forEach(function (beneficiario) {
                        if (!beneficiariosTemporarios.some(b => b.CPF === beneficiario.CPF)) {
                            beneficiariosTemporarios.push({ CPF: beneficiario.CPF, Nome: beneficiario.Nome });
                        }
                    });

                    // Exibe todos os beneficiários (persistidos + temporários) no modal
                    beneficiariosTemporarios.forEach(function (beneficiario) {
                        adicionarLinhaBeneficiario(beneficiario.CPF, beneficiario.Nome);
                    });
                },
                error: function () {
                    alert("Erro ao carregar beneficiários.");
                }
            });
        } else {
            console.log("ID do cliente não definido. Beneficiários não serão carregados.");
            beneficiariosTemporarios.forEach(function (beneficiario) {
                adicionarLinhaBeneficiario(beneficiario.CPF, beneficiario.Nome);
            });
        }
    });

    $('#btnIncluirBeneficiario').on('click', function () {
        var cpf = $('#beneficiarioCPF').val();
        var nome = $('#beneficiarioNome').val().trim();

        if (!cpf || !nome) {
            alert("Por favor, preencha todos os campos.");
            return;
        }

        // Verifica duplicidade de CPF na lista temporária
        if (beneficiariosTemporarios.some(b => b.CPF === cpf)) {
            alert("O CPF já foi adicionado.");
            return;
        }

        beneficiariosTemporarios.push({ CPF: cpf, Nome: nome });
        adicionarLinhaBeneficiario(cpf, nome);

        limparCamposModal();
        $('#beneficiariosModal').modal('hide');
    });

    function adicionarLinhaBeneficiario(cpf, nome) {
        var newRow = `
            <tr>
                <td class="cpf">${cpf}</td>
                <td class="nome">${nome}</td>
                <td>
                    <button type="button" class="btn btn-primary btn-sm btnAlterar">Alterar</button>
                    <button type="button" class="btn btn-danger btn-sm btnExcluir">Excluir</button>
                </td>
            </tr>
        `;
        $('#beneficiariosList').append(newRow);
    }

    $(document).on('click', '.btnAlterar', function () {
        editRow = $(this).closest('tr');
        var cpf = editRow.find('.cpf').text();
        var nome = editRow.find('.nome').text();

        $('#beneficiarioCPF').val(cpf);
        $('#beneficiarioNome').val(nome);
        $('#beneficiariosModal').modal('show');
    });

    $(document).on('click', '.btnExcluir', function () {
        var row = $(this).closest('tr');
        var cpf = row.find('.cpf').text();

        beneficiariosTemporarios = beneficiariosTemporarios.filter(b => b.CPF !== cpf);
        row.remove();
        limparCamposModal();
    });

    function limparCamposModal() {
        $('#beneficiarioCPF').val('');
        $('#beneficiarioNome').val('');
        editRow = null;
    }
});
