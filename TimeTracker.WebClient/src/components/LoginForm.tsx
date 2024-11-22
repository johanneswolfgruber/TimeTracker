import { useState } from "react";
import { FormButton, TextButton } from "./Buttons";
import { useNavigate } from "react-router";
import { useAuthentication } from "../context/AuthenticationContext";

export const LoginForm = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const auth = useAuthentication();
  const navigate = useNavigate();

  const login = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!auth || email === "" || password === "") {
      alert("Please provide a valid email and password");
      return;
    }

    await auth.loginAction({ data: { email, password } });
  };

  return (
    <div className="-mt-16 flex h-screen">
      <div className="m-auto w-[400px]">
        <form
          onSubmit={login}
          className="p-8 flex flex-col gap-4 bg-surface0 rounded-lg shadow-xl"
        >
          <input
            className="bg-surface0 border-b border-text focus:outline-none"
            onChange={(e) => setEmail(e.target.value)}
            type="email"
            placeholder="Email"
            id="email"
            value={email}
          />
          <input
            className="bg-surface0 border-b border-text focus:outline-none"
            onChange={(e) => setPassword(e.target.value)}
            type="password"
            placeholder="Password"
            id="password"
            value={password}
          />
          <FormButton>Login</FormButton>
          <TextButton onClick={() => navigate("/signup")}>Sign Up</TextButton>
        </form>
      </div>
    </div>
  );
};
