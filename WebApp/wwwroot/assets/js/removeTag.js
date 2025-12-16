window.addEventListener('load', () => {

    const $modal = $('#remove_tag_modal');
    let tagRemoveId = null;

    // --- Open modal ---
    $('.remove_tag_button').on('click', (e) => {
        e.preventDefault();

        const $btn = $(e.currentTarget);
        tagRemoveId = $btn.closest('.menu-item').data('remove-id');

        $modal.show().css('opacity', 1);
    });

    // --- Cancel ---
    $('.remove_tag_form_button_cancel').on('click', () => {
        $modal.hide().css('opacity', 0);
    });

    // --- Confirm ---
    $('.remove_tag_form_button_confirm').on('click', () => {

        $.ajax({
            url: `/admin/RemoveTag?tagId=${tagRemoveId}`,
            type: 'POST',
            data: { tagId: tagRemoveId }, // <-- ось
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },

            success: function (response) {
                console.log("Удаление тега:", response);

                if (response.code === 200) {
                    location.reload(); // 🔄 Обновить таблицу
                } else {
                    alert("Не удалось удалить тег");
                }

                $modal.hide().css('opacity', 0);
            },

            error: function (xhr) {
                console.error("Ошибка при удалении тега", xhr);
                alert("Ошибка при удалении");
                $modal.hide().css('opacity', 0);
            }
        });
    });

});
