document.getElementById('media').addEventListener('change', async () => {
    var mediaInput = document.getElementById('media');
    const url = await GetImageString(mediaInput);
    var mediaContainer = document.getElementById('mediaContainer');
    if (!mediaContainer) {
        mediaContainer = document.createElement('div');
        mediaContainer.id = 'mediaContainer';
        mediaContainer.style.position = 'absolute';
        mediaContainer.style.left = '70px';
        mediaContainer.style.height = '3.75rem';

        mediaContainer.innerHTML = `
        <ul id='mediaList'>
            <li>
                <img src="${url}" class="media-image" />
                <img src="/x-mark.png" alt="remove" class="image-removal" />
            </li>
        </ul>`;

        document.getElementById('chat-footer').appendChild(mediaContainer);
        document.getElementById('messageInput').disabled = true;
        document.getElementById('messageInput').placeholder = '';
    } else {
        const media = document.createElement('li');
        media.innerHTML = `
        <img src="${url}" class="media-image" />
        <img src="/x-mark.png" alt="remove" class="image-removal" />`;

        document.getElementById('mediaList').appendChild(media);
    }
});

function chooseMedia() {
    document.getElementById('media').click();
}

function isSendingMedia() {
    return !!document.getElementById('mediaContainer');
}