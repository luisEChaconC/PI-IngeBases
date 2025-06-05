class DateService {
    constructor() {
        this.testDate = null;
    }

    async getCurrentDate() {
        if (this.testDate) {
            return new Date(this.testDate);
        }
        return new Date();
    }

    async fetchDateFromBackend() {

    }

    getMondayOfWeek(date) {
        const currentDayOfWeek = date.getDay();
        const mondayOffset = currentDayOfWeek === 0 ? -6 : 1 - currentDayOfWeek;

        const mondayDate = new Date(date);
        mondayDate.setDate(date.getDate() + mondayOffset);
        return mondayDate;
    }

    generateWeekDays(referenceDate) {
        const mondayDate = this.getMondayOfWeek(referenceDate);
        const dayNames = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];

        return dayNames.map((name, index) => {
            const dayDate = new Date(mondayDate);
            dayDate.setDate(mondayDate.getDate() + index);

            return {
                name,
                date: dayDate,
                dayIndex: index,
                isWeekend: index >= 5, // Sábado y Domingo
                isoString: dayDate.toISOString().split('T')[0] // YYYY-MM-DD
            };
        });
    }

    getCurrentDayName(date) {
        const dayNames = ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'];
        return dayNames[date.getDay()];
    }

    isDateBefore(date1, date2) {
        const d1 = new Date(date1.getFullYear(), date1.getMonth(), date1.getDate());
        const d2 = new Date(date2.getFullYear(), date2.getMonth(), date2.getDate());
        return d1 < d2;
    }

    isSameDate(date1, date2) {
        return date1.getFullYear() === date2.getFullYear() &&
            date1.getMonth() === date2.getMonth() &&
            date1.getDate() === date2.getDate();
    }

    formatDate(dateString) {
        const date = new Date(dateString);
        return date.toLocaleDateString('es-ES', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        });
    }

    formatDateTime(dateString) {
        if (!dateString) return 'N/A';
        const date = new Date(dateString);

        console.log('Date object toString():', date.toString());
        console.log('Date object getTimezoneOffset():', date.getTimezoneOffset());

        return date.toLocaleString('es-ES', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    getDayName(dateString) {
        const date = new Date(dateString);
        const dayNames = ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'];
        return dayNames[date.getDay()];
    }

    getTimeAgo(dateString) {
        if (!dateString) return 'N/A';

        const now = new Date();
        const date = new Date(dateString); // Ensure this is interpreted as UTC if the string format supports it
        const diffMs = now.getTime() - date.getTime(); // Compare timestamps in milliseconds (independent of timezone for the diff)
        const diffMins = Math.floor(diffMs / 60000);
        const diffHours = Math.floor(diffMins / 60);
        const diffDays = Math.floor(diffHours / 24);

        if (diffMins < 1) return 'Hace menos de un minuto';
        if (diffMins < 60) return `Hace ${diffMins} minuto${diffMins !== 1 ? 's' : ''}`;
        if (diffHours < 24) return `Hace ${diffHours} hora${diffHours !== 1 ? 's' : ''}`;
        if (diffDays < 7) return `Hace ${diffDays} día${diffDays !== 1 ? 's' : ''}`;

        // For dates older than 7 days, show just the date.
        return date.toLocaleDateString('es-ES', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        });
    }

}

const dateService = new DateService();

export default dateService;