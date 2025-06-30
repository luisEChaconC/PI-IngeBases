import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { PdfFile } from './pdfFile';

export async function generatePdfFromElement(element, filename = 'document.pdf') {
    try {
        const canvas = await html2canvas(element, {
            useCORS: true,
            scale: 2,
            allowTaint: true
        });

        const imgData = canvas.toDataURL('image/png');

        const pdf = new jsPDF({
            orientation: 'portrait',
            unit: 'pt',
            format: 'a4'
        });

        const pdfWidth = pdf.internal.pageSize.getWidth();
        const pdfHeight = (canvas.height * pdfWidth) / canvas.width;
        pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);

        return new PdfFile(filename, pdf);
    } catch (error) {
        throw new Error('Error generating PDF');
    }
}