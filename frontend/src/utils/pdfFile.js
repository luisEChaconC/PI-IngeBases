export class PdfFile {
    constructor(filename, wrapee) {
        this.filename = filename;
        this.wrapee = wrapee;
    }

    triggerUserDownload() {
        this.wrapee.save(this.filename);
    }

    getBase64() {
        return this.wrapee.output('datauristring');
    }
}