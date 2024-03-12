let dataTable;

$(document).ready(() => {
    dataTable = $('#datatable-category').DataTable({
        "ajax": {
            "type": "GET",
            "url": "/librarian/category/getAllCategories"
        },
        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": "categoryId",
                "render": (data) => {
                    return `
                        <div class="text-center">
                            <a class="btn btn-success text-white"
                                href="/librarian/category/upsert/${data}">
                                <span class="fas fa-edit"></span>&nbsp;Sửa
                            </a>
                            <a class="btn btn-danger text-white"
                                onclick=Delete("/librarian/category/deleteCategory/${data}")>
                                <span class="fas fa-trash-alt"></span>&nbsp;Xóa
                            </a>
                        </div>
                        `;
                },
                "width": "40%"
            }
        ]
    });
});