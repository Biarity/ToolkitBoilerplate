
export default function IsVisible(element: any, partiallyVisible: Boolean = false): Boolean {
    if (element) {
        const rect = element.getBoundingClientRect();
        const elemTop = rect.top;
        const elemBottom = rect.bottom;

        // Fully visible
        let isVisible = (elemTop >= 0) && (elemBottom <= window.innerHeight);

        // Partially visible
        isVisible = partiallyVisible ? elemTop < window.innerHeight && elemBottom >= 0 : isVisible;

        return isVisible;
    }

    return false;
}
