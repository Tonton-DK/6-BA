# BA
Bachelor project

# Local Docker Run With Local Images
```cd /```

```docker compose build```

```docker compose up --build```

go to http://localhost:8080/

# Local Docker Run With External Images
```cd /```

```docker-compose -f docker-compose-github.yml up```

go to http://localhost:8080/

# Local Kubernetes Run With External Images
Install Docker Desktop + Kind

```cd /```

```kind create cluster```

```kubectl create secret docker-registry regcred --docker-server=ghcr.io --docker-username=antje12 --docker-password=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 --docker-email=antje12@student.sdu.dk```

```kubectl apply -f Pipeline/Kubernetes/deploy-frontend.yml```
```kubectl apply -f Pipeline/Kubernetes/deploy-user.yml```
```kubectl apply -f Pipeline/Kubernetes/deploy-job.yml```
```kubectl apply -f Pipeline/Kubernetes/deploy-offer.yml```
```kubectl apply -f Pipeline/Kubernetes/deploy-contract.yml```
```kubectl apply -f Pipeline/Kubernetes/deploy-review.yml```
```kubectl apply -f Pipeline/Kubernetes/deploy-metric-server.yaml```

```kubectl port-forward services/frontend 80:80```

go to http://localhost:80/

```kind delete cluster```
