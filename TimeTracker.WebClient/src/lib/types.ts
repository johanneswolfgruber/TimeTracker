import { TimeSpan } from "../utils/TimeSpan";

export interface ITracking {
  id: string;
  activityId: string;
  startTime: Date;
  endTime?: Date;
  duration: string;
  notes?: string;
}

export interface ITrackingFormData {
  startTime: Date;
  endTime?: Date;
  duration: TimeSpan;
  notes?: string;
}

export interface IActivity {
  id: string;
  userId: string;
  name: string;
  trackings: ITracking[];
}

export interface IUser {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
}

export interface ISignupData {
  data: {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
  };
}

export interface ILoginData {
  data: {
    email: string;
    password: string;
  };
}

export interface IToken {
  expiryDate: Date;
  token: string;
}

export interface ILoginResponseData {
  user: IUser;
  token: IToken;
}

export interface IApiResponse<T = void> {
  isSuccess: boolean;
  message: string;
  value?: T;
}

export interface IApiErrorResponse extends IApiResponse {
  errors: string;
}
