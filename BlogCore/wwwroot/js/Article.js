var datatable;

$(document).ready(function () {

    LoadDatataTable();

});


function LoadDatataTable() {
    datatable = $("#tblAticle").DataTable({
        "ajax": {
            "url": "/Admin/Article/GetAllArticles",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idArticle", "width": "5%" },
            { "data": "name", "width": "25%" },
            { "data": "category.name", "width": "15%" },
            { "data": "creationDate", "width": "15%" },
            {
                "data": "idArticle",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Article/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                <i class="far fa-edit"></i>Editar
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Article/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="far fa-trash-alt"></i>Borrar
                                </a>
                            </div>
                            `;
                }, "width": "30%"
            }
        ],
        "width": "100%"

    });
}


function Delete(url) {




    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });

}