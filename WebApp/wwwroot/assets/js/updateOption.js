window.addEventListener('load', () => {

    const $modal = $('#add_relation_modal');

    $('.add_more_relation_button').on('click', () => {
        $modal.show();
        $modal.css('opacity', 1);
    })

    // Cancel
    $('.add_relation_form_button_cancel').on('click', () => {
        $modal.hide();
        $modal.css('opacity', 0);
    });

    // Confirm
    $('.add_relation_form_button_confirm').on('click', () => {
        //<input class="input" name="new_relation" type="text" />
        const $newRelationInput = $modal.find('input[name="new_relation"]');
        let text = $newRelationInput.val();
        if (text.length <= 2) {
            // alert
            return;
        }

        $('.edit-option-form .reations-select').append(`<option value="${text}">${text}</option>`);
        $newRelationInput.val("");
        $modal.hide();
        $modal.css('opacity', 0);
    })
})