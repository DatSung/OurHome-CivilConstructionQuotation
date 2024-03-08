﻿var dataTableCM


$(document).ready(function () {
    loadDataCustomer();
});


//change name method - remember
//Need an api method return json to use this
function loadDataCustomer() {
    dataTableCM = $('#tblCustomer').DataTable({
        "ajax": { url: '/Manager/Customer/GetAll' },
        "columns": [

            { data: 'id', "width": "30%" },
            { data: 'name', "width": "30%" },
            {
                data: null,
                render: function (data) {
                    return `<div class="text-center">
                        <a class="btn btn-main" onclick="fillForm('${data.id}',' ${data.name}')">Add</a>
                    </div>`
                }
            }
            
        ]
    });
}



function fillForm(id, name) {
    var idField = document.getElementById('CustomerId');
    var nameField = document.getElementById('CustomerName');
    idField.value = id;
    nameField.value = name;
    idField.ajax.reload();
    nameField.ajax.reload();
}
