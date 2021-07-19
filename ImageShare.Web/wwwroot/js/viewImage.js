$(() => {
    setInterval(() => {
        updateLikes();
    }, 1000);

    $("#like-button").on('click', function () {
        const id = $("#image-id").val();
        $.post('/Home/likeImage', { id }, function () {
            updateLikes();
            $("#like-button").prop('disabled', true);
        });
    });

    function updateLikes() {
        const id = $("#image-id").val();
        $.get('/Home/getLikes', { id }, function (result) {
            $("#likes-count").text(result.likes);
        });
    }
});