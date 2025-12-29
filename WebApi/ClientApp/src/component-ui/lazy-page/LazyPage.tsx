import pMinDelay from 'p-min-delay';
import React, { lazy, Suspense } from 'react';
import LoaderPage from '../loader-page';
export const Loadable = (importFunc: any, fallback: any) => {
    const LazyComponent = lazy(importFunc);

    return (props: any) => (
        <Suspense fallback={fallback}>
            <LazyComponent {...props} />
        </Suspense>
    );
};

const LazyPage = (importFunc: any) => {
    const LazyComponent = lazy(() => pMinDelay(importFunc, 250));

    return () => (
        <Suspense fallback={<LoaderPage />}>
            <LazyComponent />
        </Suspense>
    );
}
export default LazyPage