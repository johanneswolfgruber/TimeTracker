import { createContext, useContext } from "react";
import { ILoginData, IUser } from "./../lib/types";

type AuthenticationContextData = {
  user: IUser | null;
  token: string;
  loginAction: (data: ILoginData) => Promise<void>;
  logoutAction: () => void;
};

export const AuthenticationContext =
  createContext<AuthenticationContextData | null>(null);

export const useAuthentication = () => {
  return useContext(AuthenticationContext);
};
