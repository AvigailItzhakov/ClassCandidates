$(() => {
    const id = $("#id").val();

    $("#confirmButton").on('click', function () {
        $.get('/home/confirm', { id }, function () {
            $.get('/home/getcounts', counts => {
                $("#pending-count").text(counts.pending);
                $("#confirmed-count").text(counts.confirmed);
                $("#declined-count").text(counts.declined);

            });
            $("#confirmButton").hide();
            $("#declineButton").hide();
            console.log('confirmed');
        })
    })

    $("#declineButton").on('click', function () {
        $.post('/home/decline', { id }, function () {
            $.get('/home/getcounts', counts => {
                $("#pending-count").text(counts.pending);
                $("#confirmed-count").text(counts.confirmed);
                $("#declined-count").text(counts.declined);

            })
            $("#confirmButton").hide();
            $("#declineButton").hide();
        })
    })

})