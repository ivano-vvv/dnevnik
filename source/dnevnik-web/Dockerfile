FROM node:18-alpine

RUN npm i pnpm -g

WORKDIR /app

COPY . ./

RUN pnpm install

CMD ["pnpm", "dev"]
