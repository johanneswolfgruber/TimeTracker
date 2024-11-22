import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { ILoginData, IUser } from "../lib/types";
import { fetchUser, loginUser } from "../lib/api";
import { AuthenticationContext } from "../context/AuthenticationContext";

type AuthenticationProviderProps = {
  children: React.ReactNode;
};

export const AuthenticationProvider = ({
  children,
}: AuthenticationProviderProps) => {
  const [user, setUser] = useState<IUser | null>(null);
  const [token, setToken] = useState(localStorage.getItem("token") || "");
  const navigate = useNavigate();

  useEffect(() => {
    let ignore = false;

    if (token) {
      const fetch = async () => {
        if (ignore) {
          return;
        }

        const res = await fetchUser(token);
        if (res.isSuccess && res.value) {
          setUser(res.value);
        }
      };

      fetch().catch(console.error);

      return () => {
        ignore = true;
      };
    }
  }, [token]);

  const loginAction = async (data: ILoginData) => {
    const res = await loginUser(data);
    if (res.isSuccess && res.value?.user && res.value?.token) {
      setUser(res.value.user);
      setToken(res.value.token.token);
      localStorage.setItem("token", res.value.token.token);
      navigate("/");
    }
  };

  const logoutAction = () => {
    setUser(null);
    setToken("");
    localStorage.removeItem("token");
    navigate("/login");
  };

  return (
    <AuthenticationContext.Provider
      value={{ user, token, loginAction, logoutAction }}
    >
      {children}
    </AuthenticationContext.Provider>
  );
};
