import { toast } from 'react-toastify';
import hotToast from 'react-hot-toast';
type NotifyInputType = {
    message: string,
    type: "info" | "warning" | "success" | "error" | "default",
    displayTime?: number

}

export const showNotify = (props: NotifyInputType) => {
    toast(props.message, {
        type: props.type,
        hideProgressBar: true,
        autoClose: props.displayTime || 3000,
        // position: "bottom-center",
        position: "top-center",
        theme: "colored"
    })
}

export const showHotToast = (props: any) => {
    hotToast(props.message, {
        duration: 3000,
        position: 'top-center',

        // type: "success",

        // Styling
        style: {},
        className: '',

        // Custom Icon
        icon: '👏',

        // Change colors of success/error/loading icon
        iconTheme: {
            primary: '#000',
            secondary: '#fff',
        },

        // Aria
        ariaProps: {
            role: 'status',
            'aria-live': 'polite',
        },
    });
}

export const NotifyHelper = {
    //https://react-hot-toast.com/docs/toast
    Success: (message: string) => hotToast.success(message),
    Error: (message: string) => hotToast.error(message),
    Warning: (message: string) => hotToast.error(message)

    // Success: (message: string) => showNotify({ message: message, type: "success" }),
    // Error: (message: string) => showNotify({ message: message, type: "error" }),
    // Warning: (message: string) => showNotify({ message: message, type: "warning" })
}