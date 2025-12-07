(() => {

    // 1. Рендер одного коментаря (рекурсія)
    let renderOneComment = (commObj, level = 0) => {

        let childrenHtml = "";
        if (commObj.children && commObj.children.length > 0) {
            childrenHtml = commObj.children
                .map(child => renderOneComment(child, level + 1))
                .join('');
        }

        return `
        <div class="comment-item comment-level-${level}">
        
            <div class="d-flex mb-3">
                <img src="${commObj.UserAvatar}" 
                     class="img-fluid rounded" 
                     style="width: 45px; height: 45px;">

                <div class="ps-3">
                    <h6>
                        <a href="mailto:${commObj.UserEmail}">
                            ${commObj.UserLogin}
                        </a>
                        <small><i>
                            ${new Date(commObj.DateOfCreated).toLocaleDateString('uk-UA', {
                                day: 'numeric',
                                month: 'long',
                                year: 'numeric'
                            })}
                        </i></small>
                    </h6>

                    <p>${commObj.Text}</p>

                <button class="btn btn-sm btn-light btn-reply" data-id="${commObj.Id}">
                    Відповісти
                </button>

                </div>
            </div>

            <div class="children">
                ${childrenHtml}
            </div>

        </div>`;
    };


    // 2. Зібрати масив у дерево
    function buildTree(list) {
        let map = {};
        let roots = [];

        list.forEach(c => {
            c.children = [];
            map[c.Id] = c;
        });

        list.forEach(c => {
            if (c.ParentCommentId === null) {
                roots.push(c);
            } else {
                map[c.ParentCommentId]?.children.push(c);
            }
        });

        return roots;
    }



    // 3. Рендер всього дерева
    let renderCommentsTree = (tree, $container) => {
        $container.empty();
        tree.forEach(c => {
            $container.append(renderOneComment(c));
        });
    };



    // 4. Завантаження коментарів
    function loadComments() {

        let slug = $('.postSlug').text();
        let cont = $('.comments-container-body');

        $.get(`/ajax/GetAllComments?slug=${slug}`, (response) => {

            let respObj = JSON.parse(response);

            if (respObj.Code === 200 && respObj.Status === "Success") {
                let comments = JSON.parse(respObj.Data);

                let tree = buildTree(comments);

                $('.section-title h3').text(`Комментарів - ${comments.length}`);
                renderCommentsTree(tree, cont);
            }
        }).fail(function (xhr, status, error) {
            console.error("Error loading comments:", error);
        });
    }


    // 5. Клік на Reply
    $(document).on("click", ".btn-reply", function () {
        let parentId = $(this).data('id');

        $(".parentCommentId").val(parentId);

        $("html, body").animate({
            scrollTop: $(".bg-light").offset().top - 100
        }, 300);
    });


    // 6. Надсилання нового коментаря
    $("form").on("submit", function (e) {
        e.preventDefault();

        let dto = {
            UserLogin: $("#userName").val(),
            UserEmail: $("#userEmail").val(),
            UserAvatar: "/images/avatars/anon.png",
            Text: $("#commentText").val(),
            PostId: parseInt($(".postId").text()),
            ParentCommentId: $(".parentCommentId").val() || null
        };

        $.ajax({
            url: "/ajax/AddComment",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(dto),
            success: function (resp) {

                let r;
                try {
                    r = typeof resp === "string" ? JSON.parse(resp) : resp;
                } catch {
                    r = resp;
                }

                if (r.Code === 200) {

                    $(".comment-success-msg")
                        .removeClass("d-none")
                        .hide()
                        .fadeIn(300);

                    setTimeout(() => {
                        $(".comment-success-msg").fadeOut(400);
                    }, 3000);

                    $("form")[0].reset();
                    $(".parentCommentId").val("");

                    //чукаємо модерації а не зразу додаємо
                    //loadComments();  
                }
            }


        });
    });


    // 7. Підвантажити коментарі після завантаження сторінки
    window.addEventListener('load', () => loadComments());


})();
