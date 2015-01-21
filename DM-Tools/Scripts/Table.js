var table;
var userName;

$(function () {
    do {
        userName = prompt('Enter your name:', '');
    } while (userName == '');

    table = $.connection.tableHub;
    table.client.showMessage = showMessage;

    table.client.showChatMessage = function (name, message) {
        $('#chat').append('<li><strong>' + htmlEncode(name) + '</strong>: ' + htmlEncode(message) + '</li>');
    };

    table.client.showDiceRoll = function (user, roll) {
        $('#chat').append('<li><em>' + htmlEncode(user) + ' rolls: ' + roll + '</em></li>');
    };

    table.client.setupUi = function (isDM) {
        console.log(isDM ? "You are the DM" : "You are not the DM");
    };

    $.connection.hub.start().done(setupConnection);

    $.connection.hub.disconnected(function () {
        console.log("Disconnected from server. Reconnecting...");
        setTimeout(function () { $.connection.hub.start().done(setupConnection); }, 2000);
    });

    $('#message').keypress(function(e) {
        if(e.keyCode==13)
            $('#sendmessage').click();
    });
});

function setupConnection() {
    table.server.join(tableID, userName);
    $('#message').focus();

    $('#sendmessage').click(function () {
        var msg = $('#message').val().trim();
            
        if ($.connection.hub.state != $.signalR.connectionState.connected) {
            console.log("Error, not connected. State:" + $.connection.hub.state);
            return;
        }
        if (msg != '')
            table.server.say(tableID, msg);
        $('#message').val('').focus();
    });
}

function showMessage(message) {
    $('#chat').append('<li><em>' + htmlEncode(message) + '</em></li>');
}

function htmlEncode(value) {
    return $('<div />').text(value).html();
}