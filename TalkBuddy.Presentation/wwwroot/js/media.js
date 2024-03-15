
var mediaConnection = new signalR.HubConnectionBuilder().withUrl("/media").build();
mediaConnection.start().then(function () {
    console.log("Media Connection started");
}).catch(function (err) {
    return console.error(err.toString());
});
async function HandleReport(userId) {
    const form = document.getElementById(`reportForm_${userId}`);
    const Id = form.querySelector('input[name="UserId"]').value;
    const detailSelect = form.querySelector('#reason');
    const detail = detailSelect.options[detailSelect.selectedIndex].value;
    const fileInput = document.getElementById('fileInput');
    try {
        const url = await GetImageString(fileInput);
        console.log(url);
        mediaConnection.invoke("SendReport",Id,detail,url)
        local.href=("/ChatPage")
    } catch (error) {
        console.error(error);
    }                                                            
}
