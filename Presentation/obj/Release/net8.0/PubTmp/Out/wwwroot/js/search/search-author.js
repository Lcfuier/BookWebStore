$(() => {
    $('#author-name').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/librarian/author/searchAuthors",
                data: {
                    "term": request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $('#author-id').val(ui.item.id);
        }
    });
});