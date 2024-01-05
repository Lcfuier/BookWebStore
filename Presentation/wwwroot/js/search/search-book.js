$(() => {
    $('#book-title').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/book/searchBooks",
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
            $('#book-id').val(ui.item.id);
        }
    });
});