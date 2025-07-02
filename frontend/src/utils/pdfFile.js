import EmailService from '@/services/EmailService';

const BODY = 'Se adjunta el archivo PDF';

export class PdfFile {
    constructor(filename, wrapee) {
        this.filename = filename;
        this.wrapee = wrapee;
    }

    triggerUserDownload() {
        this.wrapee.save(this.filename);
    }

    sendToEmail(subject, to) {
        EmailService.sendEmail({
            to: to,
            subject: subject,
            body: BODY,
            attachmentBase64: this.getBase64(),
            attachmentFilename: this.filename
        });
    }

    sendToCurrentUserEmail(subject) {
        const currentUserInformation = JSON.parse(localStorage.getItem('currentUserInformation'));
        const email = currentUserInformation?.email;
        if (email) {
            this.sendToEmail(subject, email);
        }
    }

    getBase64() {
        const dataUri = this.wrapee.output('datauristring');
        // Remove the "data:application/pdf;base64," prefix to get only the base64 content
        return dataUri.split(',')[1];
    }
}