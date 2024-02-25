﻿var dataTableM;
$(document).ready(function () {
    loadDataTableMaterial();
});

//Need an api method return json to use this
function loadDataTableMaterial() {
    dataTableM = $('#tblMaterial').DataTable({
        "ajax": { url: '/Engineer/Material/GetAll' },
        "columns": [
            { data: 'id', },
            { data: 'name', },
            { data: 'inventoryQuantity', },
            { data: 'unitPrice', },
            { data: 'unit', },
            { data: 'categoryName', },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-100 btn-group" role="group">
                       <a onClick=AddToQuoteMaterial('/Engineer/Material/AddToQuote?MaterialId=${data}') class="text-nowrap btn btn-primary btn-main border-0 m-1"><i class="bi bi-plus-square"></i> Add</a>
                    </div >`
                },
            }
        ]
    });
}

function AddToQuoteMaterial(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (data) {
            dataTableMD.ajax.reload();
            dataTableM.ajax.reload();
            toastr.success(data.message);
        },
        error: function (data) {
            dataTableMD.ajax.reload();
            dataTableM.ajax.reload();
            toastr.error(data.message);
        }
    });
}