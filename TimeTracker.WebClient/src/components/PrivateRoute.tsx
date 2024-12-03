import { Navigate, Outlet } from "react-router";
import { useAuthentication } from "../context/AuthenticationContext";

export const PrivateRoute = () => {
  const auth = useAuthentication();
  if (!auth?.token) {
    return <Navigate to="/login" />;
  }

  return <Outlet />;
};

export default PrivateRoute;
