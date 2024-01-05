let dataTable;

$(document).ready(() => {
    dataTable = $('#datatable-book').DataTable({
        "ajax": {
            "type": "GET",
            "url": "/librarian/book/getAllBooks"
        },
        "columns": [
            { "data": "title", "width": "20%" },
            { "data": "price", "width": "10%" },
            { "data": "author", "width": "20%" },
            { "data": "categories", "width": "20%" },
            {
                "data": "bookId",
                "render": (data) => {
                    return `
                        <div class="text-center">
                            <a class="btn btn-success text-white"
                                href="/librarian/book/upsert/${data}">
                                <span class="fas fa-edit"></span>&nbsp;Sửa
                            </a>
                            <a class="btn btn-danger text-white"
                                onclick=Delete("/librarian/book/deleteBook/${data}")>
                                <span class="fas fa-trash-alt"></span>&nbsp;Xóa
                            </a>
                        </div>
                        `;
                },
                "width": "20%"
            }
        ]
    });
});