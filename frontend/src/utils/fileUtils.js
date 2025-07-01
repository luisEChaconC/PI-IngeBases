import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { PdfFile } from './pdfFile';

const PIXEL_TO_PT = 0.75; // 1 px = 0.75 pt

export async function generatePdfFromElement(element, filename = 'document.pdf') {
    try {
        const canvas = await html2canvas(element, {
            useCORS: true,
            scale: 2,
            allowTaint: true
        });

        const imgData = canvas.toDataURL('image/png');

        const pdfWidth = canvas.width * PIXEL_TO_PT;
        const pdfHeight = canvas.height * PIXEL_TO_PT;

        const pdf = new jsPDF({
            orientation: pdfWidth > pdfHeight ? 'landscape' : 'portrait',
            unit: 'pt',
            format: [pdfWidth, pdfHeight]
        });

        pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);

        return new PdfFile(filename, pdf);
    } catch (error) {
        throw new Error('Error generating PDF');
    }
}