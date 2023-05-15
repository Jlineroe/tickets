
$(function () {
    var chat = $.connection.chatHub;
    chat.client.sendChat = function (Notifi) {
        var divFecha = $('<div class="small text-gray-500" />').text(Notifi.DateLog).html();
        var divMessage = $('<span class="font-weight-bold" />').text(Notifi.Message).html();
        $("#divContAlert").append(`<a class="dropdown-item d-flex align-items-center" href="${Notifi.Link}">
            <div class="mr-3">
                <div class="icon-circle bg-primary">
                    <i class="fas fa-plus-circle text-white"></i>
                </div>
            </div>
            <div>${divFecha + divMessage}</div>
        </div>`);
    };

    $.connection.hub.start().done(function () {
        $("#sendmessage").click(function () {
            var nameOwner = "Boddy1";
            var messageText = $("#message").val();

            chat.server.send(nameOwner, messageText);
            $("#message").val("").focus();
        })
    })
})