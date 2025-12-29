import { useMemo } from 'react'
import { useAppSelector } from './useAppSelector'

export const useLocalized = (code: string) => {
    const { localized_resources } = useAppSelector(x => x.common.localizedResourceReducer)


    const localizedValue = useMemo(() => {
        // console.log({
        //     code,
        //     value: localized_resources.get(code)
        // });

        return localized_resources.get(code) ?? code;
    }, [code, localized_resources])

    return localizedValue;
}