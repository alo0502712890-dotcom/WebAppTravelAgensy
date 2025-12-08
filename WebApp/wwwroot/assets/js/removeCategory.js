window.addEventListener('load', () => {
    const $modal = $('#remove_category_modal');

    let categoryRemoveId = null;

    // Open modal
    $('.remove_category_button').on('click', (e) => {
        categoryRemoveId = $(e.currentTarget).data('remove-id');

        $modal.show();
        $modal.css('opacity', 1);
    });

    // Cancel
    $('.remove_category_form_button_cancel').on('click', () => {
        $modal.hide();
        $modal.css('opacity', 0);
    });

    // Confirm
    $('.remove_category_form_button_confirm').on('click', () => {

        $.ajax({
            url: `/admin/RemoveCategory?categoryId=${categoryRemoveId}`,
            type: 'GET',
            success: function (response) {
                console.log("Удалено успешно", response);
                response = JSON.parse(response);

                $modal.hide();
                $modal.css('opacity', 0);

                if (response.Status == "Success") {
                    // 👉 если нужно обновить страницу:
                    location.reload();
                } else {
                    // modal error
                }
                // 👉 или удалить элемент из UI без перезагрузки
                // $(`#option_${optionRemoveId}`).remove();
            },
            error: function (xhr) {
                console.error("Ошибка при удалении", xhr);
                alert("Ошибка при удалении");
            }
        });
        $modal.hide();
        $modal.css('opacity', 0);
    });
})