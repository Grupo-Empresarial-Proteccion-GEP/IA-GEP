// wwwroot/js/imageHelper.js

export async function corregirOrientacionYConvertir(base64String) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        img.onload = function () {
            EXIF.getData(img, function () {
                const orientation = EXIF.getTag(this, "Orientation") || 1;

                const canvas = document.createElement('canvas');
                const ctx = canvas.getContext('2d');

                let width = img.width;
                let height = img.height;

                if ([5, 6, 7, 8].includes(orientation)) {
                    canvas.width = height;
                    canvas.height = width;
                } else {
                    canvas.width = width;
                    canvas.height = height;
                }

                switch (orientation) {
                    case 2: ctx.transform(-1, 0, 0, 1, width, 0); break;
                    case 3: ctx.transform(-1, 0, 0, -1, width, height); break;
                    case 4: ctx.transform(1, 0, 0, -1, 0, height); break;
                    case 5: ctx.transform(0, 1, 1, 0, 0, 0); break;
                    case 6: ctx.transform(0, 1, -1, 0, height, 0); break;
                    case 7: ctx.transform(0, -1, -1, 0, height, width); break;
                    case 8: ctx.transform(0, -1, 1, 0, 0, width); break;
                    default: break;
                }

                ctx.drawImage(img, 0, 0);

                const base64Corregido = canvas.toDataURL('image/jpeg');
                resolve(base64Corregido);
            });
        };
        img.onerror = reject;
        img.src = base64String;
    });
}
