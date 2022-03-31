@startuml
skinparam shadowing false

actor User as u
participant UI as ui
participant ProfileService as p
participant JobService as j
participant OfferService as o
participant ContractService as c
participant ReviewService as r

u -> ui
activate ui

ui -> p: ValidateUser(email,password)
activate p

ui <-- p: sessionToken
deactivate p

u <-- ui
u -> ui

ui -> j: CreateJob(Job)
activate j

ui <-- j: job
deactivate j

u <-- ui
u -> ui

ui -> o: ListOffersForJob(jobId)
activate o

ui <-- o: offers

u <-- ui
u -> ui

ui -> o: AcceptOffer(Id)
o -> c: CreateContract(Offer)
activate c

o <-- c: contract

ui <-- o: contract
deactivate o

u <-- ui
u -> ui

ui -> c: ConcludeContract(id)

ui <-- c: contract
deactivate c

u <-- ui
u -> ui

ui -> r: CreateReview(Review)
activate r
ui <-- r: review
deactivate r

u <-- ui
deactivate ui

@enduml