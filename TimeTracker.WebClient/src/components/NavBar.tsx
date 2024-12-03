import { ClockIcon, PowerIcon } from "@heroicons/react/24/solid";
import { NavLink } from "react-router";
import { useAuthentication } from "../context/AuthenticationContext";

export const NavBar = () => {
  const auth = useAuthentication();
  const isAuthenticated = !!auth?.token;

  const onLogout = () => {
    if (auth) {
      auth.logoutAction();
    }
  };

  return (
    <nav className="fixed top-0 w-full bg-base z-50">
      <NavLink to="/">
        <div className="h-16 px-4 py-6 flex flex-row items-center justify-between border-b border-surface1 shadow-xl">
          <div className="flex flex-row items-center justify-start">
            <ClockIcon className="w-10 h-10" />
            <h1 className="ml-4 text-2xl font-bold tracking-widest">
              Time Tracker
            </h1>
          </div>
          <div>
            {isAuthenticated && (
              <div className="flex flex-row items-center justify-end">
                <h3 className="mr-4 text-sm tracking-widest">
                  {auth.user?.firstName}
                </h3>
                <button onClick={onLogout} className="text-text outline-none">
                  <PowerIcon className="w-10 h-10" />
                </button>
              </div>
            )}
          </div>
        </div>
      </NavLink>
    </nav>
  );
};
