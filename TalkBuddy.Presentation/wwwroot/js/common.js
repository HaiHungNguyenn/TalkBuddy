
async function GetImageString(fileInput) {
    const formData = new FormData();
    if (fileInput.files.length > 0) {
        const file = fileInput.files[0];
        formData.append('ImageFile', file);
    }
    try {
        const response = await fetch('https://domus.io.vn/test', {
            method: 'POST',
            body: formData
        });
        if (!response.ok) {
            alert('Network response was not ok');
            throw new Error('Network response was not ok');
        }
        const data = await response.text();
        return data;
    } catch (error) {
        alert(err.message);
        console.error('There was a problem with the fetch operation:', error);
        throw error; 
    }
}
