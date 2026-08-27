import React, { Suspense, lazy } from "react";
import { Box } from "@primer/react";
import { Route, Switch } from "react-router-dom";
import UnAuthorizedContent from "./UnAuthorizedContent";
import LoaderPage from "../../component-ui/loader-page";

// Lazy để lỗi/module trang tra cứu không làm trắng trang login
const TraCuuPage = lazy(() => import("../../pages/tra-cuu"));
const TraCuuChungTuPage = lazy(() => import("../../pages/tra-cuu-chung-tu"));
const HoaDonViewPage = lazy(() => import("../../pages/hoa-don-view"));

const publicRoutes = [
  { path: "/tra-cuu", component: TraCuuPage },
  { path: "/tra-cuu-chung-tu", component: TraCuuChungTuPage },
  { path: "/hoa-don/view/:id", component: HoaDonViewPage },
];

const PublicContent = () => {
  return (
    <Box>
      <Suspense fallback={<LoaderPage />}>
        <Switch>
          {publicRoutes.map(({ path, component }) => (
            <Route strict key={path} path={path} component={component} />
          ))}
          <Route component={UnAuthorizedContent} />
        </Switch>
      </Suspense>
    </Box>
  );
};

export default PublicContent;
