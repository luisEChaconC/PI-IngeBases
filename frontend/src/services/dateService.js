class DateService {
    constructor() {
        this.testDate = '2024-01-30';
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
}

const dateService = new DateService();

export default dateService;