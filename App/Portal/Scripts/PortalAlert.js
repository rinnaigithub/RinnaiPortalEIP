function pNotifySuccessAlert(msg) {
    new PNotify({
        title: 'success!!',
        text: msg,
        type: 'success'
    });
}

function pNotifyAlert(msg) {
    new PNotify({
        title: '提示!!',
        text: msg,
        type: 'error'
    });
}

function confirmAlert(msg, delegate) {
    //return false;
    $.confirm({
        type: 'orange',
        typeAnimated: true,
        theme: 'supervan',
        title: '提醒!',
        content: msg,
        buttons: {
            confirm: {
                text: '確定',
                action: delegate
            },
        }
    });
}