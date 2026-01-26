// console.log(process.env.REACT_APP_API_BASE_URL);

interface IAppInfo {
  baseApiURL: string;
  chuKySoSignalrUrl: string;
}

const appInfo: IAppInfo = {
  baseApiURL: process.env.REACT_APP_API_BASE_URL?.toString() ?? "",
  // chuKySoSignalrUrl: "http://127.0.0.1",
  chuKySoSignalrUrl: "http://127.0.0.1:5000/chathub",
};

export { appInfo };
