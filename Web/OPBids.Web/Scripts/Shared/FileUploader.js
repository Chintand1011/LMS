var FileUploader = {
    TotalParts: null,
    TotalCount: null,
    UploadUrl: null,
    Status: true,
    Sleep: function (miliseconds) {
        var e = new Date().getTime() + (miliseconds);
        while (new Date().getTime() <= e) { }
    },
    CurData: null,
    GetFileName: function (fileName) {
        return [XFW.GetSession(CFW.UserGUID), '-', Math.floor(Date.now() / 1000), '.', fileName.substring(fileName.lastIndexOf('.') + 1)].join('');
    },
    ConvertToBase64: function (files) {
        var deferred = $.Deferred();
        if (files.length > 0) {
            var fr = new FileReader();
            fr.onload = function (e) {
                var rslt = e.target.result;
                if (rslt == "data:") {
                    rslt = "";
                }
                deferred.resolve(rslt);
            };
            fr.readAsDataURL(files[0]);
        } else {
            deferred.resolve(undefined);
        }
        return deferred.promise();
    },
    Base64toBlob: function (base64Data, contentType, fileName) {
        if (base64Data instanceof Blob) {
            return Blob
        }
        else {
            contentType = [contentType, '/', fileName.substring(fileName.lastIndexOf('.') + 1)].join('')
            var sliceSize = 1024;
            var byteCharacters = atob(base64Data);
            var bytesLength = byteCharacters.length;
            var slicesCount = Math.ceil(bytesLength / sliceSize);
            var byteArrays = new Array(slicesCount);
            for (var sliceIndex = 0; sliceIndex < slicesCount; ++sliceIndex) {
                var begin = sliceIndex * sliceSize;
                var end = Math.min(begin + sliceSize, bytesLength);
                var bytes = new Array(end - begin);
                for (var offset = begin, i = 0 ; offset < end; ++i, ++offset) {
                    bytes[i] = byteCharacters[offset].charCodeAt(0);
                }
                byteArrays[sliceIndex] = new Uint8Array(bytes);
            }
            return new Blob(byteArrays, { type: contentType });
        }
    },
    ConvertUrlToBase64: function (url, callback) {
        var xhr = new XMLHttpRequest();
        xhr.onload = function () {
            var reader = new FileReader();
            reader.onloadend = function () {
                callback(reader.result);
            }
            reader.readAsDataURL(xhr.response);
        };
        xhr.onreadystatechange = function () {
            if (this.status == 200) {
            }
        };
        if ("withCredentials" in xhr) {
            xhr.open('GET', url, true);
        } else if (typeof XDomainRequest != "undefined") {
            xhr = new XDomainRequest();
            xhr.open('GET', url, true);
            xhr.setRequestHeader("Access-Control-Allow-Origin", "*");
            xhr.setRequestHeader("Access-Control-Allow-Headers", "*");
        } else {
            xhr = null;
        }
        if (xhr != null) {
            xhr.responseType = 'blob';
            xhr.send();
        }
    },
    UploadFileChunk: function (Chunk, fileName, folder, async, postEvt) {
        var data = new FormData();
        data.append("file", Chunk, fileName);
        $.ajax({
            type: "POST",
            url: ["/Shared/UploadFile", "?filename=", fileName, "&folder=", folder].join(''),
            contentType: false,
            processData: false,
            data: data,
            async: async,
            success: function (rslts) {
                FileUploader.TotalCount++;
                if (postEvt != null && FileUploader.TotalParts <= FileUploader.TotalCount) {
                    postEvt();
                }
            },
            error: function (xhr, status, error) {
                FileUploader.TotalCount++;
                FileUploader.Status = false;
                Site.Dialogs.Alert("An error has occurred.", "Ok", null);
            }
        });
    },
    UploadFile: function (blobFile, folder, fileName, async, postEvt) {
        FileUploader.Status = true;//reset
        if (blobFile != null && blobFile != undefined && fileName != null && fileName != undefined) {
            // create array to store the buffer chunks  
            var FileChunk = [];
            // the file object itself that we will work with  
            // set up other initial vars  
            var MaxFileSizeMB = 2;
            var BufferChunkSize = MaxFileSizeMB * (1024 * 1024);
            var ReadBuffer_Size = 1024;
            var FileStreamPos = 0;
            // set the initial chunk length  
            var EndPos = BufferChunkSize;
            var Size = blobFile.size;

            // add to the FileChunk array until we get to the end of the file  
            while (FileStreamPos < Size) {
                // "slice" the file from the starting position/offset, to  the required length  
                FileChunk.push(blobFile.slice(FileStreamPos, EndPos));
                FileStreamPos = EndPos; // jump by the amount read  
                EndPos = FileStreamPos + BufferChunkSize; // set next chunk length  
            }
            // get total number of "files" we will be sending  
            FileUploader.TotalParts = FileChunk.length;
            var PartCount = 0;
            // loop through, pulling the first item from the array each time and sending it  
            while (chunk = FileChunk.shift()) {
                    PartCount++;
                    // file name convention  
                    var FilePartName = fileName + ".part_" + PartCount + "." + FileUploader.TotalParts;
                    // send the file  
                    FileUploader.UploadFileChunk(chunk, FilePartName, folder, async, postEvt);
                    FileUploader.Sleep(400);
                }
            }
    },
    Folders: {
        Document: "Document",
    },
    GetDocument: function (filename) {
        if (filename) {
            if (filename.length < 100) {
                return [FileUploader.UploadUrl, "/", FileUploader.Folders.Document, "/", filename].join('');
            }
            else {
                return filename;
            }
        }
    },
    GetPFMSDocument: function (filename) {
        if (filename) {
            if (filename.length < 100) {
                return [FileUploader.UploadUrl, "/", filename].join('');
            }
            else {
                return filename;
            }
        }
    },
    UploadDocument: function (blobFile, fileName, opts, async, postEvt) {
        var targetFolder = FileUploader.Folders.Document;
        async = typeof async !== 'undefined' ? async : false;
        if (opts) {
            if (opts.dir) {
                targetFolder = opts.dir;
            }
        }
        FileUploader.UploadFile(blobFile, targetFolder, fileName, async, postEvt);
    },
}
