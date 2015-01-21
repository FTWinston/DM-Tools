var table;
var userName;

$(function () {
    $('#tableContent .modeTab').first().addClass('active');
    $('#tableContent .modeSelector li').click(tabClicked).first().addClass('selected');

    do {
        userName = prompt('Enter your name:', '');
    } while (userName == '');

    initChat();
    initWhiteboard();
});

function tabClicked() {
    if ($(this).hasClass('selected'))
        return;

    $('#tableContent .modeSelector li.selected').removeClass('selected');
    $('#tableContent .modeTab.active').removeClass('active');
    var target = $(this).addClass('selected').attr('target');
    $('#' + target).addClass('active');
}
