type DefaultAnimationProps = {
    of?: string
}
export const getDefaultAnimation = (props: DefaultAnimationProps): any => {
    return {
        show: {
            type: 'fade',
            duration: 0,
            from: 0,
            to: 0
        },
        // hide: {
        //     type: 'slide',
        //     duration: 400,
        //     from: {
        //         position: { my: 'center', at: 'center', of: window }
        //     },
        //     to: { position: { my: 'center', at: 'bottom', of: props.of || window } },
        // }
    }
    // return {
    //     show: {
    //         type: 'slide',
    //         duration: 400,
    //         from: {
    //             position: { my: 'center', at: 'center', of: props.of || window, }
    //         },
    //         to: {
    //             position: { my: 'center', at: 'center', of: window }
    //         },
    //     },
    //     hide: {
    //         type: 'slide',
    //         duration: 400,
    //         from: {
    //             position: { my: 'center', at: 'center', of: window }
    //         },
    //         to: { position: { my: 'center', at: 'bottom', of: props.of || window } },
    //     }
    // }
}

