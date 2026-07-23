export function extractErrorMessage(err) {
    const data = err.response?.data;

    if (!data) return "Network error. Try again.";

    if (data.errors) {
        const messages = Object.values(data.errors).flat();
        if (messages.length > 0) return messages[0];
    }

    if (typeof data.detail === "string") return data.detail;
    if (typeof data.title === "string") return data.title;

    return "Something went wrong.";
}